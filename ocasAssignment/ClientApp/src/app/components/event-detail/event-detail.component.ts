import { Component, OnInit } from '@angular/core';
import { EventService } from '../../services/event.service';
import { Event } from '../../models/Event';
import { FormGroup, FormBuilder, FormControl, Validators } from '@angular/forms';
import { ActivatedRoute } from '@angular/router';
import { Employee } from '../../models/Employee';
import { Subscription, Subject } from 'rxjs';
import { EventDetailsDTO } from '../../models/EventDetailsDTO';
import { EventDetailsResponseDTO } from '../../models/EventDetailsResponseDTO';
import { EventDetailsResultDialogComponent } from '../event-details-result-dialog/event-details-result-dialog.component';
import { MatDialog } from '@angular/material/dialog';

@Component({
    selector: 'app-event-detail',
    templateUrl: './event-detail.component.html',
    styleUrls: ['./event-detail.component.css']
})
/** eventDetail component*/
export class EventDetailComponent implements OnInit {

  events: Event[];

  displayedColumns: string[] = ['FirstName', 'LastName', 'EmailAddress'];

  employees: Employee[];
  selectedEventId: number;
  selectedEvent: Event;

  eventsObservable: Subject<number>;
  eventsSubscription: Subscription;

  eventDetailsResponse: EventDetailsResponseDTO;

  eventDetailsForm: FormGroup;

  constructor(private eventService: EventService,
    private formBuilder: FormBuilder,
    private activatedRoute: ActivatedRoute,
    public dialog: MatDialog) {

  }

  /** ngOnDestroy Method */
  ngOnDestroy() {
    this.eventsSubscription.unsubscribe();
  }

  /** ngOnInit() method */
  ngOnInit() {
    this.eventsObservable = new Subject<number>();
    this.eventChangeSubscriptionHandler();

    this.getEvents();
    this.createForm();

    this.activatedRoute.params.subscribe(params => {
      if (params.id) {
        this.eventsObservable.next(params.id);
      }
    });

  }

  /**
   * This function will display an error dialog to the user
   * @param eventDetailsResponse The response from the server
   */
  showErrorDialog(eventDetailsResponse: EventDetailsResponseDTO) {
    this.dialog.open(EventDetailsResultDialogComponent, {
      width: '500px',
      data: eventDetailsResponse
    });
  }

  /** When a new Event is selected this function will obtain the details for that Event */
  eventChangeSubscriptionHandler() {
    this.eventsSubscription = this.eventsObservable.asObservable().subscribe((eventId: number) => {

      this.eventService.getEvent(eventId).then((event: Event) => {
        this.selectedEvent = event;
      });
      
    });
  }

  /** This function will obtain all the Events from the server */
  getEvents() {
    this.eventService.getAllEvents().then((events: Event[]) => {
      this.events = events;
    });
  }

  /** This function creates the form for submitting event detail requests */
  createForm() {
    this.eventDetailsForm = this.formBuilder.group({
      event: new FormControl('', { validators: [Validators.required] }),
      emailAddress: new FormControl('', { validators: [Validators.required] })
    });
  }

  /** This function is called when the user submits the form.
   * It will obtain all the Employees from the server as well if the user is authorized to view the details */
  onSubmit() {

    let emailAddress = this.eventDetailsForm.get('emailAddress').value;
    let event = <Event>this.eventDetailsForm.get('event').value;
    this.eventsObservable.next(event.id);

    let eventDetails = <EventDetailsDTO>{
      eventId: event.id,
      emailAddress: emailAddress
    };

    this.eventService.getEmployeesForEvent(eventDetails).then((eventDetailsResponse: EventDetailsResponseDTO) => {

      this.eventDetailsResponse = eventDetailsResponse

      if (eventDetailsResponse.result) {
        this.employees = eventDetailsResponse.employees;
      }
      else {
        this.showErrorDialog(eventDetailsResponse);
      }

    });

  }


  /**************************** FORM **************************************/

  getFormControl(name: string) {
    return this.eventDetailsForm.get(name);
  }

  isValid(name: string) {
    var e = this.getFormControl(name);
    return e && e.valid;
  }

  isChanged(name: string) {
    var e = this.getFormControl(name);
    return e && (e.dirty || e.touched);
  }

  hasError(name: string) {
    var e = this.getFormControl(name);
    return e && (e.dirty || e.touched) && !e.valid;
  }

/***********************************************************************/

}
