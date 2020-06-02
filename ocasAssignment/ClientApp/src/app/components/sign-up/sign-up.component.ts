import { Component, OnInit } from '@angular/core';
import { MatDialog, MatDialogRef, MAT_DIALOG_DATA } from '@angular/material/dialog';

import { FormControl, Validators, FormGroup, FormBuilder } from '@angular/forms';
import { Event } from '../../models/Event';
import { EventService } from '../../services/event.service';
import { EmployeeSignUp } from '../../models/EmployeeSignUp';
import { SignUpService } from '../../services/sign-up.service';
import { EmployeeSignUpResult } from '../../models/employeeSignUpResult';
import { SignUpResultDialogComponent } from '../sign-up-result-dialog/sign-up-result-dialog.component';

@Component({
    selector: 'app-sign-up',
    templateUrl: './sign-up.component.html',
    styleUrls: ['./sign-up.component.css']
})
/** sign-up component*/
export class SignUpComponent implements OnInit {

  firstName: string;
  lastName: string;
  emailAddress: string;
  comments: string;

  events: Event[];
  selectedEvent: Event;

  signUpResult: EmployeeSignUpResult;

  signUpForm: FormGroup;
  
  constructor(private formBuilder: FormBuilder,
    private eventService: EventService,
    private signUpService: SignUpService,
    public dialog: MatDialog) {

  }

  /** ngOnInit() method */
  ngOnInit() {
    this.setEvents();
    this.createForm();
  }

  /** This method gets all the events from the server and sets the events field*/
  setEvents() {
    this.eventService.getAllEvents().then((events: Event[]) => {
      this.events = events;
    });
  }

  /** This function creates the Sign Up Form. */
  createForm() {

    this.signUpForm = this.formBuilder.group({
      firstName: new FormControl('', {
        validators: [Validators.required, Validators.minLength(2), Validators.maxLength(16)]
      }),

      lastName: new FormControl('', {
        validators: [Validators.required, Validators.minLength(2), Validators.maxLength(16)]
      }),

      emailAddress: new FormControl('', {
        validators: [Validators.required, Validators.email]
      }),

      event: new FormControl('', {
        validators: [Validators.required]
      }),

      comments: new FormControl('')
    });

  }

  /** This function is called when the user submits the form.
   *  A dialog is opened after the Sign Up process is complete to notify the user of the result of the operation. */
  onSubmit() {
    let employeeSignUp = <EmployeeSignUp>{
      firstName: this.signUpForm.get('firstName').value,
      lastName: this.signUpForm.get('lastName').value,
      emailAddress: this.signUpForm.get('emailAddress').value,
      eventId: (<Event>this.signUpForm.get('event').value).id,
      comments: this.signUpForm.get('comments').value
    };

    this.signUpService.addEmployeeToEvent(employeeSignUp).then((signUpResult: EmployeeSignUpResult) => {

      this.signUpResult = signUpResult;

      this.dialog.open(SignUpResultDialogComponent, {
        width: '500px',
        data: signUpResult
      });
    });
  }

  /**************************** FORM **************************************/

  getFormControl(name: string) {
    return this.signUpForm.get(name);
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
