// Retires the service worker that earlier builds registered for this app (Uno.Wasm.Bootstrap's
// PWA mode, scope /uno-wasm-demos/). The app no longer registers one, but browsers keep an existing
// registration and re-check this URL on every visit, so this script replaces it: it activates at
// once, has no fetch handler (requests go straight to the network), deletes the old precache and
// unregisters itself.
self.addEventListener('install', () => self.skipWaiting());

self.addEventListener('activate', (event) => {
  event.waitUntil((async () => {
    try {
      // Cache Storage is shared by every app on this origin. The bootstrapper names its precache
      // with a GUID, so delete only GUID-named caches that hold this app's URLs. The .NET runtime's
      // own "dotnet-resources-..." cache is left alone.
      const guid = /^[0-9a-f]{8}-[0-9a-f]{4}-[0-9a-f]{4}-[0-9a-f]{4}-[0-9a-f]{12}$/i;
      const scope = self.registration.scope;
      for (const name of await caches.keys()) {
        if (!guid.test(name)) continue;
        const cache = await caches.open(name);
        const requests = await cache.keys();
        if (requests.some((request) => request.url.startsWith(scope))) {
          await caches.delete(name);
        }
      }
    } finally {
      // Unregister even if the cleanup fails, so no worker stays behind.
      await self.registration.unregister();
    }
  })());
});
