import { Component, Input, Output, EventEmitter } from '@angular/core';

@Component({
  selector: 'app-map',
  templateUrl: './map.component.html',
  styleUrls: ['./map.component.css']
})
export class MapComponent {
  @Input() latitude: number;
  @Input() longitude: number;
  @Input() zoom: number = 15;
  @Input() markerDraggable: boolean = true;
  @Input() isInfoWindowOpen: boolean = false;
  @Input() infoWindowMessage: string;

  @Output() markerMoved = new EventEmitter<MouseEvent>();
}
