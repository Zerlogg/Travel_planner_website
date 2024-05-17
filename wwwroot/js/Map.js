window.initializeMap = async function () {
    const list = [];
    var map = L.map('map').setView([56.941978, 24.120483], 4);

    var pinsResponse = await fetch('/api/pins');
    var pins = await pinsResponse.json();

    pins.forEach(pin => {
        var marker = L.marker([pin.latitude, pin.longitude]).addTo(map);
        marker.on('click', async function() {
            var deleteResponse = await fetch(`/api/pins?latitude=${pin.latitude}&longitude=${pin.longitude}`, {
                method: 'DELETE'
            });
            map.removeLayer(marker);
        });
    });

    var customBounds = L.latLngBounds(
        L.latLng(-140, -160),
        L.latLng(140, 160)
    );

    map.setMaxBounds(customBounds);
    map.on('drag', function() {
        map.panInsideBounds(customBounds, { animate: false });
    });

    var Basemap = L.tileLayer('https://tile.openstreetmap.org/{z}/{x}/{y}.png', {
        maxZoom: 16,
        minZoom: 3
    }).addTo(map);

    list.forEach(element => {
        element.map(x => x.longitude);
        console.log(element);
    });

    map.on("click", function(e){
        var latitude = e.latlng.lat;
        var longitude = e.latlng.lng;

        savePinToDatabase(latitude, longitude);

        var marker = new L.marker([latitude, longitude]).addTo(map).on('click', e=> e.target.remove());
    })

    var timezones = L.timezones.bindPopup(function (layer) {
        return layer.feature.properties.time_zone;
    }).addTo(map);

    var baseMaps = {
        'Base map' : Basemap
    }

    var overlayMaps = {
        'Time zones' : timezones
    }

    L.control.layers(baseMaps, overlayMaps).addTo(map);
};

function savePinToDatabase(latitude, longitude) {
    fetch('/api/pins', {
        method: 'POST',
        headers: {
            'Content-Type': 'application/json',
        },
        body: JSON.stringify({ latitude: latitude, longitude: longitude }),
    })
        .then(response => {
            if (!response.ok) {
                throw new Error('Failed to save pin to the database. Server responded with status: ' + response.status);
            }
        })
        .catch(error => {
            console.error('Error while saving pin to the database:', error);
        });
}