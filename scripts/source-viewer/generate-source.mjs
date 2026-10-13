import { readdir, readFile, writeFile, mkdir } from 'node:fs/promises';
import path from 'node:path';
import { fileURLToPath } from 'node:url';
import { createHash } from 'node:crypto';
import { createSourceHighlighter, languages, renderSource } from './viewer.mjs';

const scriptDir = path.dirname(fileURLToPath(import.meta.url));
const repoRoot = path.resolve(scriptDir, '../..');
const samplesRoot = path.resolve(process.argv[2] ?? path.join(repoRoot, 'samples'));
const browserRoot = path.resolve(process.argv[3] ?? path.join(repoRoot, 'browser/SamplesBrowser'));
const outputRoot = path.join(browserRoot, 'Assets/Source');
const skippedDirs = new Set(['bin', 'obj', 'platforms', 'properties', 'strings', '.git']);
const scaffolding = new Set(['App.xaml', 'App.xaml.cs', 'MainWindow.xaml', 'MainWindow.xaml.cs', 'MainPage.xaml', 'MainPage.xaml.cs', 'GlobalUsings.cs', 'AssemblyInfo.cs']);
const slash = file => file.split(path.sep).join('/');

async function listFiles(dir) {
  const result = [];
  for (const entry of await readdir(dir, { withFileTypes: true })) {
    if (entry.isSymbolicLink()) continue;
    const full = path.join(dir, entry.name);
    if (entry.isDirectory() && !skippedDirs.has(entry.name.toLowerCase())) result.push(...await listFiles(full));
    else if (entry.isFile()) result.push(full);
  }
  return result.sort();
}

const allFiles = await listFiles(samplesRoot);
const fileSet = new Set(allFiles);
const sampleDirs = allFiles.filter(f => path.basename(f) === 'Sample.xaml' && fileSet.has(f + '.cs')).map(f => path.dirname(f));
const highlighter = await createSourceHighlighter();
const rendererHash = createHash('sha256').update(await readFile(path.join(scriptDir, 'viewer.mjs'))).update(await readFile(path.join(scriptDir, 'package-lock.json'))).digest('hex');
let previous = { samples: [] };
try { previous = JSON.parse(await readFile(path.join(outputRoot, 'index.json'), 'utf8')); } catch (error) { if (error.code !== 'ENOENT') throw error; }
const previousByRoute = new Map(previous.samples.map(s => [s.route, s]));
const manifest = { version: 1, samples: [] };
let generated = 0;
try {
  for (const dir of sampleDirs) {
    const route = slash(path.relative(samplesRoot, dir));
    const sourceFiles = allFiles.filter(f => f.startsWith(dir + path.sep) && languages[path.extname(f)] && !scaffolding.has(path.basename(f)));
    sourceFiles.sort((a, b) => {
      const rank = f => path.basename(f) === 'Sample.xaml' ? 0 : path.basename(f) === 'Sample.xaml.cs' ? 1 : 2;
      return rank(a) - rank(b) || slash(path.relative(dir, a)).localeCompare(slash(path.relative(dir, b)), 'en');
    });
    const files = await Promise.all(sourceFiles.map(async file => ({ name: slash(path.relative(dir, file)), language: languages[path.extname(file)], text: (await readFile(file, 'utf8')).replace(/^\uFEFF/, '') })));
    const digest = createHash('sha256').update(rendererHash).update(JSON.stringify(files)).digest('hex');
    const outputFile = path.join(outputRoot, route + '.html');
    let exists = false;
    if (previousByRoute.get(route)?.digest === digest) { try { await readFile(outputFile); exists = true; } catch (error) { if (error.code !== 'ENOENT') throw error; } }
    if (!exists) {
      await mkdir(path.dirname(outputFile), { recursive: true });
      await writeFile(outputFile, renderSource(highlighter, route, files), 'utf8');
      generated++;
    }
    manifest.samples.push({ route, digest, files: files.map(({ name, language }) => ({ name, language })) });
  }
  await mkdir(outputRoot, { recursive: true });
  await writeFile(path.join(outputRoot, 'index.json'), JSON.stringify(manifest, null, 2) + '\n');
  console.log(`Source viewer: ${sampleDirs.length} samples, ${generated} regenerated from original files.`);
} finally { highlighter.dispose(); }
