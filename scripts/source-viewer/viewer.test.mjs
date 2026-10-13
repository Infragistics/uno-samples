import { test, before, after } from 'node:test';
import assert from 'node:assert/strict';
import { JSDOM } from 'jsdom';
import { createSourceHighlighter, renderSource } from './viewer.mjs';

let highlighter;
before(async () => { highlighter = await createSourceHighlighter(); });
after(() => highlighter.dispose());

const files = [
  { name: 'Sample.xaml', language: 'xml', text: '<UserControl x:Class="Standalone.Sample">\r\n</UserControl>\r\n' },
  { name: 'Sample.xaml.cs', language: 'csharp', text: 'namespace Standalone;\n// </script><script>window.injected=true</script>\n' },
  { name: 'Data.cs', language: 'csharp', text: 'public class Data { public string Value => "data"; }\n' },
];

function openViewer() {
  return new JSDOM(renderSource(highlighter, 'charts/category-chart/overview', files), {
    runScripts: 'dangerously',
    beforeParse(window) { window.HTMLElement.prototype.scrollTo = () => {}; },
  });
}

test('highlights original XAML, codebehind and data without executing source markup', () => {
  const dom = openViewer();
  assert.equal(dom.window.injected, undefined);
  assert.equal(dom.window.document.querySelectorAll('script').length, 1);
  assert.equal(dom.window.document.querySelectorAll('[role="tab"]').length, 3);
  assert.match(dom.window.document.querySelector('#file-0 code').textContent, /Standalone.Sample/);
  assert.ok(dom.window.document.querySelector('#file-0 code span[class^="t"]'));
  assert.equal(dom.window.document.querySelector('#file-1 code').textContent, files[1].text);
  assert.match(dom.window.document.querySelector('#file-2 code').textContent, /class Data/);
  assert.equal(dom.window.document.querySelectorAll('script[src],link[href]').length, 0);
  dom.window.close();
});

test('file selection supports clicks and arrow/Home/End keys', () => {
  const dom = openViewer();
  const { document, KeyboardEvent } = dom.window;
  const tabs = Array.from(document.querySelectorAll('[role="tab"]'));
  tabs[1].click();
  assert.equal(document.querySelector('#file-1').hidden, false);
  assert.equal(document.querySelector('#file-0').hidden, true);
  tabs[1].dispatchEvent(new KeyboardEvent('keydown', { key: 'ArrowRight', bubbles: true }));
  assert.equal(document.activeElement, tabs[2]);
  assert.equal(tabs[2].getAttribute('aria-selected'), 'true');
  tabs[2].dispatchEvent(new KeyboardEvent('keydown', { key: 'Home', bubbles: true }));
  assert.equal(document.activeElement, tabs[0]);
  tabs[0].dispatchEvent(new KeyboardEvent('keydown', { key: 'End', bubbles: true }));
  assert.equal(document.activeElement, tabs[2]);
  dom.window.close();
});

test('copy preserves exact original text, including CRLF and trailing newline', async () => {
  const dom = openViewer();
  let copied;
  Object.defineProperty(dom.window.navigator, 'clipboard', { value: { writeText: async text => { copied = text; } } });
  dom.window.document.getElementById('copy').click();
  await new Promise(resolve => setTimeout(resolve, 0));
  assert.equal(copied, files[0].text);
  dom.window.document.getElementById('tab-2').click();
  dom.window.document.getElementById('copy').click();
  await new Promise(resolve => setTimeout(resolve, 0));
  assert.equal(copied, files[2].text);
  dom.window.close();
});

test('copy works without clipboard APIs in an embedded WebView', async () => {
  const dom = openViewer();
  let copied;
  dom.window.document.execCommand = command => { assert.equal(command, 'copy'); copied = dom.window.document.querySelector('textarea').value; return true; };
  dom.window.document.getElementById('tab-1').click();
  dom.window.document.getElementById('copy').click();
  await new Promise(resolve => setTimeout(resolve, 0));
  assert.equal(copied, files[1].text);
  assert.equal(dom.window.document.querySelector('textarea'), null);
  dom.window.close();
});
