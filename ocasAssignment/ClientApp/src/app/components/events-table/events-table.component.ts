import { Component, Input, OnInit } from '@angular/core';
import { EventService } from '../../services/event.service';
import { Event } from '../../models/Event';

@Component({
    selector: 'app-events-table',
    templateUrl: './events-table.component.html',
    styleUrls: ['./events-table.component.css']
})
/** events-table component*/
export class EventsTableComponent implements OnInit {

  displayedColumns: string[] = ['Name', 'StartDate', 'EndDate'];

  events: Event[];

  constructor(private eventService: EventService) {

  }

  /** ngOnInit() method */
  ngOnInit() {
    this.getAllEvents();
  }

  /** This method will get all the events from the server */
  getAllEvents() {
    this.eventService.getAllEvents().then((events: Event[]) => {
      this.events = events;
    });
  }

}
