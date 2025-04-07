export class OpenStreetMapHelper {
    static dotNetHelper;
    static mapId = 'dMap';
    static map = null;
    static marker = null;

    static Init(dotNetRef, mapContainerId) {
        OpenStreetMapHelper.dotNetHelper = dotNetRef;
        if (mapContainerId)
            OpenStreetMapHelper.mapId = mapContainerId;
    }

    static async ShowMap(lat = null, lng = null, zoom = null) {
        console.log('loading the map for the edited project');
        const mapContainer = document.getElementById(OpenStreetMapHelper.mapId);

        if (typeof L === 'undefined'
            || !L
            || !mapContainer)
            return;

        //параметры по умолчанию
        if (!lat || !lng) {
            lat = 55.7504461;
            lng = 37.6174943;
        }
        if (!zoom) zoom = 12;

        const latlng = [lat, lng];

        //показываем контейнер
        mapContainer.style.display = 'block';

        //загружаем карту
        if (OpenStreetMapHelper.map === null) {
            OpenStreetMapHelper.map = L.map(mapContainer).setView(latlng, zoom);

            // реклама (без этого карта работать не будет)
            L.tileLayer('https://{s}.tile.openstreetmap.org/{z}/{x}/{y}.png', {
                attribution: '© <a href="https://www.openstreetmap.org/copyright">OpenStreetMap</a> contributors'
            }).addTo(OpenStreetMapHelper.map);

            //отслеживаем зум
            OpenStreetMapHelper.map.on('zoomend', async (e) => {
                console.log('Zoom', e.sourceTarget._zoom);
                OpenStreetMapHelper
                    .dotNetHelper
                    .invokeMethodAsync(
                        'UpdateGeoZoomAsync',
                        e.sourceTarget._zoom);
            });
        }
        else {
            // перемещение к следующей позиции
            OpenStreetMapHelper.map.flyTo(latlng);

            // удаление предыдущего маркера
            if (OpenStreetMapHelper.marker)
                OpenStreetMapHelper.map.removeLayer(OpenStreetMapHelper.marker);
        }

        // добавляем маркер
        OpenStreetMapHelper.marker = new L.Marker(latlng, {
            draggable: true
        }).addTo(OpenStreetMapHelper.map);

        //отслеживаем координаты
        OpenStreetMapHelper.marker.on('dragend', async (e) => {
            let position = OpenStreetMapHelper.marker.getLatLng();
            console.log('LatLng', position);
            await OpenStreetMapHelper
                .dotNetHelper
                .invokeMethodAsync(
                    'UpdateGeoLatLngAsync',
                    position.lat,
                    position.lng);
        });
    }
}

window.OpenStreetMapHelper = OpenStreetMapHelper;