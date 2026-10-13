import { test } from 'node:test';
import assert from 'node:assert/strict';
import { mkdtemp, mkdir, writeFile, readFile, rm } from 'node:fs/promises';
import { tmpdir } from 'node:os';
import path from 'node:path';
import { fileURLToPath } from 'node:url';
import { execFile } from 'node:child_process';
import { promisify } from 'node:util';
import { JSDOM } from 'jsdom';

const run = promisify(execFile);
const generator = fileURLToPath(new URL('./generate-source.mjs', import.meta.url));

test('generation preserves each original sample, excludes scaffolding, and refreshes changed data', async () => {
  const tempBase = path.resolve(tmpdir());
  const temp = await mkdtemp(path.join(tempBase, 'sample-source-test-'));
  const samplesRoot = path.join(temp, 'samples');
  const browserRoot = path.join(temp, 'browser');
  const originalXaml = '<UserControl x:Class="Original.Sample"/>\r\n';
  try {
    for (const sample of ['one', 'two']) {
      const dir = path.join(samplesRoot, 'charts/test', sample);
      await mkdir(path.join(dir, 'obj'), { recursive: true });
      await writeFile(path.join(dir, 'Sample.xaml'), '\uFEFF' + originalXaml);
      await writeFile(path.join(dir, 'Sample.xaml.cs'), 'namespace Original; public class Sample {}\n');
      await writeFile(path.join(dir, 'Data.cs'), `// data belonging to ${sample}\n`);
      await writeFile(path.join(dir, 'App.xaml.cs'), '// excluded scaffolding');
      await writeFile(path.join(dir, 'MainPage.xaml.cs'), '// excluded scaffolding');
      await writeFile(path.join(dir, 'obj', 'Generated.cs'), '// excluded build output');
    }
    await run(process.execPath, [generator, samplesRoot, browserRoot]);
    const manifestPath = path.join(browserRoot, 'Assets/Source/index.json');
    const manifest = JSON.parse(await readFile(manifestPath));
    assert.equal(manifest.samples.length, 2);
    for (const sample of ['one', 'two']) {
      const entry = manifest.samples.find(s => s.route === `charts/test/${sample}`);
      assert.deepEqual(entry.files.map(f => f.name), ['Sample.xaml', 'Sample.xaml.cs', 'Data.cs']);
      const html = await readFile(path.join(browserRoot, `Assets/Source/charts/test/${sample}.html`), 'utf8');
      const dom = new JSDOM(html);
      assert.equal(dom.window.document.getElementById('file-0').textContent, originalXaml.replace(/\r\n/g, '\n'));
      assert.match(dom.window.document.getElementById('file-2').textContent, new RegExp(`belonging to ${sample}`));
      dom.window.close();
    }
    const unchanged = await run(process.execPath, [generator, samplesRoot, browserRoot]);
    assert.match(unchanged.stdout, /0 regenerated/);
    await writeFile(path.join(samplesRoot, 'charts/test/one/Data.cs'), '// changed source\n');
    const changed = await run(process.execPath, [generator, samplesRoot, browserRoot]);
    assert.match(changed.stdout, /1 regenerated/);
  } finally {
    // Validate the absolute cleanup target stays inside the temporary directory.
    const relative = path.relative(tempBase, path.resolve(temp));
    assert.ok(relative.startsWith('sample-source-test-') && !relative.includes(path.sep));
    await rm(temp, { recursive: true, force: true });
  }
});
