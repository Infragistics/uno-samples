import { createHighlighter } from 'shiki';

export const languages = { '.cs': 'csharp', '.xaml': 'xml', '.xml': 'xml', '.json': 'json', '.csv': 'text' };

export function createSourceHighlighter() {
  return createHighlighter({ themes: ['github-light', 'github-dark'], langs: ['csharp', 'xml', 'json'] });
}

const escapeHtml = text => text.replace(/[&<>"']/g, c => ({ '&': '&amp;', '<': '&lt;', '>': '&gt;', '"': '&quot;', "'": '&#39;' }[c]));
const scriptJson = value => JSON.stringify(value).replace(/</g, '\\u003c').replace(/\u2028/g, '\\u2028').replace(/\u2029/g, '\\u2029');
const highlightingCache = new WeakMap();

export function renderSource(highlighter, route, files) {
  if (!highlightingCache.has(highlighter)) highlightingCache.set(highlighter, new Map());
  const cache = highlightingCache.get(highlighter);
  // Share token styles: repeated inline colors otherwise exceed WebView2's 2 MB HTML limit
  // for samples with large data files.
  const tokenStyles = new Map();
  const tabs = files.map((f, i) => `<button role="tab" id="tab-${i}" aria-controls="file-${i}" aria-selected="${i === 0}" tabindex="${i === 0 ? 0 : -1}" data-index="${i}">${escapeHtml(f.name)}</button>`).join('');
  const panels = files.map((f, i) => {
    const key = f.language + '\0' + f.text;
    if (!cache.has(key)) cache.set(key, highlighter.codeToHtml(f.text, { lang: f.language, themes: { light: 'github-light', dark: 'github-dark' } }));
    const code = cache.get(key).replace(/<span style="([^"]+)">/g, (_, style) => {
      if (!tokenStyles.has(style)) tokenStyles.set(style, 't' + tokenStyles.size);
      return `<span class=${tokenStyles.get(style)}>`;
    }).replace(/class="line"/g, 'class=line');
    return `<section role="tabpanel" id="file-${i}" aria-labelledby="tab-${i}" ${i ? 'hidden' : ''}>${code}</section>`;
  }).join('');
  return `<!doctype html>
<html lang="en"><head><meta charset="utf-8">
<meta name="viewport" content="width=device-width,initial-scale=1">
<meta http-equiv="Content-Security-Policy" content="default-src 'none'; style-src 'unsafe-inline'; script-src 'unsafe-inline'">
<title>Sample source: ${escapeHtml(route)}</title>
<style>
${Array.from(tokenStyles, ([style, name]) => '.' + name + '{' + style + '}').join('\n')}
:root { color-scheme: light dark; --bg: #fff; --fg: #24292f; --border: #d0d7de; --accent: #0969da; }
html[data-theme="dark"] { --bg: #0d1117; --fg: #e6edf3; --border: #30363d; --accent: #58a6ff; color-scheme: dark; }
html[data-theme="light"] { color-scheme: light; }
@media (prefers-color-scheme: dark) { html:not([data-theme="light"]) { --bg: #0d1117; --fg: #e6edf3; --border: #30363d; --accent: #58a6ff; } }
* { box-sizing: border-box; } body { margin: 0; background: var(--bg); color: var(--fg); font: 14px system-ui, sans-serif; height: 100vh; display: flex; flex-direction: column; }
header { display: flex; border-bottom: 1px solid var(--border); gap: 8px; align-items: center; padding: 8px; }
[role="tablist"] { display: flex; overflow-x: auto; flex: 1; gap: 4px; }
button { background: var(--bg); color: var(--fg); border: 1px solid var(--border); border-radius: 4px; padding: 8px 12px; white-space: nowrap; cursor: pointer; font: inherit; }
button[aria-selected="true"] { border-color: var(--accent); box-shadow: inset 0 -2px var(--accent); }
button:focus-visible, pre:focus-visible { outline: 2px solid var(--accent); outline-offset: -2px; }
main { flex: 1; min-height: 0; overflow: auto; } section { min-height: 100%; }
pre.shiki { margin: 0; padding: 16px; min-height: 100%; width: max-content; min-width: 100%; font: 13px/1.6 Consolas, 'SFMono-Regular', monospace; tab-size: 4; counter-reset: line; }
.line::before { counter-increment: line; content: counter(line); display: inline-block; width: 4ch; margin-right: 2ch; text-align: right; color: #8b949e; user-select: none; }
html[data-theme="dark"] .shiki, html[data-theme="dark"] .shiki span { color: var(--shiki-dark) !important; background-color: var(--shiki-dark-bg) !important; }
@media (prefers-color-scheme: dark) { html:not([data-theme="light"]) .shiki, html:not([data-theme="light"]) .shiki span { color: var(--shiki-dark) !important; background-color: var(--shiki-dark-bg) !important; } }
#status { position: absolute; width: 1px; height: 1px; overflow: hidden; clip-path: inset(50%); }
</style></head><body>
<header><div role="tablist" aria-label="Source files">${tabs}</div><button id="copy" aria-label="Copy current source file">Copy</button></header>
<main>${panels}</main><span id="status" role="status" aria-live="polite"></span>
<script>
const files = ${scriptJson(files.map(({ name, text }) => ({ name, text })))};
const tabs = Array.from(document.querySelectorAll('[role="tab"]'));
let selected = 0;
function selectFile(index, focus) {
  selected = index;
  tabs.forEach((tab, i) => { tab.setAttribute('aria-selected', String(i === index)); tab.tabIndex = i === index ? 0 : -1; document.getElementById('file-' + i).hidden = i !== index; });
  document.querySelector('main').scrollTo(0, 0);
  if (focus) tabs[index].focus();
}
tabs.forEach((tab, i) => {
  tab.addEventListener('click', () => selectFile(i, false));
  tab.addEventListener('keydown', event => {
    const next = event.key === 'ArrowRight' ? (i + 1) % tabs.length : event.key === 'ArrowLeft' ? (i + tabs.length - 1) % tabs.length : event.key === 'Home' ? 0 : event.key === 'End' ? tabs.length - 1 : -1;
    if (next >= 0) { event.preventDefault(); selectFile(next, true); }
  });
});
document.getElementById('copy').addEventListener('click', async () => {
  try {
    try { await navigator.clipboard.writeText(files[selected].text); }
    catch {
      const input = document.createElement('textarea'); input.value = files[selected].text; input.style.position = 'fixed'; input.style.opacity = '0'; document.body.appendChild(input); input.select();
      let copied; try { copied = document.execCommand('copy'); } finally { input.remove(); }
      if (!copied) throw new Error('Copy unavailable');
    }
    document.getElementById('status').textContent = 'Copied ' + files[selected].name;
  } catch { document.getElementById('status').textContent = 'Copy unavailable. Select the code and copy it manually.'; }
});
</script></body></html>`;
}
