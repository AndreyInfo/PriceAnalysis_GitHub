
export function showOpenStreetMap(lat = null, lng = null, zoom = null, mapId = null) {
    console.log('loading the map for the project');

    if (typeof L === 'undefined' || !L) return;
    if (!mapId) mapId = 'dMap';
    if (!lat || !lng) {
        lat = 55.7504461;
        lng = 37.6174943;
    }
    if (!zoom) zoom = 1;

    const mapContainer = document.getElementById(mapId),
        latlng = [lat, lng];

    if (!mapContainer) return;
    mapContainer.style.display = 'block';

    let map = L.map(mapContainer).setView(latlng, zoom);

    // реклама (без этого карта работать не будет)
    L.tileLayer('https://{s}.tile.openstreetmap.org/{z}/{x}/{y}.png', {
        attribution: '© <a href="https://www.openstreetmap.org/copyright">OpenStreetMap</a> contributors'
    }).addTo(map);

    // добавляем маркер
    if (zoom > 1)
        new L.Marker(latlng).addTo(map);
}