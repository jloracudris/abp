import { Component, EventEmitter, OnInit, Output } from '@angular/core';

@Component({
  selector: 'form-map',
  templateUrl: './map.component.html',
  styleUrls: ['./map.component.scss'],
})
export class MapsComponent implements OnInit {
  
  @Output()
  getLatAndLong = new EventEmitter<{lat: number, lng: number}>();
  
  // google maps zoom level
  zoom: number = 8;

  // initial center position for the map
  lat: number = 51.673858;
  lng: number = 7.815982;

  ngOnInit(): void {
    setTimeout(() => {
      this.getLatAndLong.emit({ lat: this.lat, lng: this.lng });  
    });
    
  }
  

  mapClicked($event: any) {
    /* this.markers.push({
      lat: $event.coords.lat,
      lng: $event.coords.lng,
      draggable: true
    }); */
  }

  clickedMarker(label: string, index: number) {
    console.log(`clicked the marker: ${label || index}`);    
  }

  markerDragEnd(m: marker, $event: any) {
    this.getLatAndLong.emit({ lat: $event.latLng.lat(), lng: $event.latLng.lng()});
  }

  markers: marker[] = [
    {
      lat: 51.673858,
      lng: 7.815982,
      label: 'A',
      draggable: true,
    },    
  ];
}

// just an interface for type safety.
interface marker {
  lat: number;
  lng: number;
  label?: string;
  draggable: boolean;
}
