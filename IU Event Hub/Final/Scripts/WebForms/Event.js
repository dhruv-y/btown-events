
$(document).ready(function () {
    // make the request
    if ($("#MainContent_addresslbl")[0].textContent!="")
        displayMap();
    
});
function displayMap() {
    var apikey = 'de49c04ce22f4ce4814a84134077ad3d';
    var address = $("#MainContent_addresslbl")[0].textContent+ ",Bloomington, Indiana";
    var location = $("#MainContent_locationlbl")[0].textContent;
    var api_url = 'https://api.opencagedata.com/geocode/v1/json';
   
    var request_url = api_url
        + '?'
        + 'key=' + apikey
        + '&q=' + encodeURIComponent(address)
        + '&pretty=1'
        + '&no_annotations=1';

    // see full list of required and optional parameters:
    // https://opencagedata.com/api#forward

    var request = new XMLHttpRequest();
    request.open('GET', request_url, true);
    request.onload = function () {
        // see full list of possible response codes:
        // https://opencagedata.com/api#codes
        debugger;
        if (request.status == 200) {
            // Success!

            var data = JSON.parse(request.responseText);
            var geometry = data.results[0].geometry;
            mapboxgl.accessToken = 'pk.eyJ1IjoiZHJpc2h0aTI0OTciLCJhIjoiY2szM3J4bDJnMHh3ajNibjk3NmJ0MmlqMyJ9.l7B4jn93C3HBeUEj7B9nOg';
            var map = new mapboxgl.Map({
                container: 'map',
                style: 'mapbox://styles/mapbox/streets-v11',
                center: [-86.5264, 39.1653],
                zoom: 12
            });
            map.on('load', function () {

                map.addLayer({
                    "id": "points",
                    "type": "symbol",
                    "source": {
                        "type": "geojson",
                        "data": {
                            "type": "FeatureCollection",
                            "features": [{
                                // feature for Mapbox DC
                                "type": "Feature",
                                "geometry": {
                                    "type": "Point",
                                    "coordinates": [geometry.lng, geometry.lat]
                                },
                                "properties": {
                                    "title": location,
                                    "icon": "monument"
                                }
                            }
                            ]
                        }
                    },
                    "layout": {
                        // get the icon name from the source's "icon" property
                        // concatenate the name to get an icon from the style's sprite sheet
                        "icon-image": ["concat", ["get", "icon"], "-15"],
                        // get the title name from the source's "title" property
                        "text-field": ["get", "title"],
                        "text-font": ["Open Sans Semibold", "Arial Unicode MS Bold"],
                        "text-offset": [0, 0.6],
                        "text-anchor": "top"
                    }
                });
            });
            
        } else if (request.status <= 500) {
            // We reached our target server, but it returned an error

            console.log("unable to geocode! Response code: " + request.status);
            var data = JSON.parse(request.responseText);
            console.log(data.status.message);
        } else {
            console.log("server error");
        }
    };

    request.onerror = function () {
        // There was a connection error of some sort
        console.log("unable to connect to server");
    };

    request.send();
    

    
}