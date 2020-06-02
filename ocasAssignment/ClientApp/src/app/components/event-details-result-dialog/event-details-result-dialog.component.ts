import { Component, Inject } from '@angular/core';
import { MatDialogRef, MAT_DIALOG_DATA } from '@angular/material/dialog';
import { EventDetailsResponseDTO } from '../../models/EventDetailsResponseDTO';

@Component({
    selector: 'app-event-details-result-dialog',
    templateUrl: './event-details-result-dialog.component.html',
    styleUrls: ['./event-details-result-dialog.component.css']
})
/** event-details-result-dialog component*/
export class EventDetailsResultDialogComponent {

  constructor(public dialogRef: MatDialogRef<EventDetailsResponseDTO>,
    @Inject(MAT_DIALOG_DATA) public data: EventDetailsResponseDTO) { }

}
