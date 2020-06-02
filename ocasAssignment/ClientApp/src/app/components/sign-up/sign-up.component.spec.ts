
import { TestBed, async, ComponentFixture, ComponentFixtureAutoDetect } from '@angular/core/testing';
import { BrowserModule, By } from "@angular/platform-browser";

import { RouterModule } from '@angular/router';

import { EventMockService } from '../../mock-services/eventMock.service';
import { SignUpMockService } from '../../mock-services/signUpMock.service';

import { EventService } from '../../services/event.service';
import { FormsModule, ReactiveFormsModule } from '@angular/forms';
import { MatTableModule } from '@angular/material/table';
import { MatSelectModule } from '@angular/material/select';
import { MatFormFieldModule } from '@angular/material/form-field';
import { MatOptionModule } from '@angular/material/core';
import { MatIconModule } from '@angular/material/icon';
import { BrowserAnimationsModule } from '@angular/platform-browser/animations';

import { SignUpComponent } from './sign-up.component';
import { EventsTableComponent } from '../events-table/events-table.component';
import { SignUpService } from '../../services/sign-up.service';
import { MAT_DIALOG_DEFAULT_OPTIONS, MatDialogModule } from '@angular/material/dialog';
import { MatInputModule } from '@angular/material/input';
import { Event } from '../../models/Event';
import { SignUpResultDialogComponent } from '../sign-up-result-dialog/sign-up-result-dialog.component';
import { BrowserDynamicTestingModule } from '@angular/platform-browser-dynamic/testing';

let component: SignUpComponent;
let fixture: ComponentFixture<SignUpComponent>;

describe('signUp component', () => {
  beforeEach(async(() => {
    TestBed.configureTestingModule({
      declarations: [
        SignUpComponent,
        EventsTableComponent,
        SignUpResultDialogComponent
      ],
      imports: [
        BrowserModule,
        BrowserAnimationsModule,
        FormsModule,
        ReactiveFormsModule,
        MatFormFieldModule,
        MatOptionModule,
        MatSelectModule,
        MatTableModule,
        MatIconModule,
        MatDialogModule,
        MatFormFieldModule,
        MatInputModule,
        RouterModule.forRoot([])
      ],
      providers: [
        { provide: ComponentFixtureAutoDetect, useValue: true },
        { provide: EventService, useClass: EventMockService },
        { provide: SignUpService, useClass: SignUpMockService },
        { provide: MAT_DIALOG_DEFAULT_OPTIONS, useValue: { hasBackdrop: false } }
      ]
    })
      .overrideModule(BrowserDynamicTestingModule, {
        set: { entryComponents: [SignUpResultDialogComponent] }
      });

    fixture = TestBed.createComponent(SignUpComponent);
    component = fixture.componentInstance;
  }));

  it('form should be valid', async(() => {

    component.createForm();

    expect(component.signUpForm.get('firstName').valid).toBeFalsy();
    expect(component.signUpForm.get('lastName').valid).toBeFalsy();
    expect(component.signUpForm.get('emailAddress').valid).toBeFalsy();
    expect(component.signUpForm.get('event').valid).toBeFalsy();
    expect(component.signUpForm.get('comments').valid).toBeTruthy();

  }));

  it('events should be valid', async(async () => {

    await component.setEvents();

    expect(3).toEqual(component.events[0].id);
    expect(4).toEqual(component.events[1].id);
    expect(5).toEqual(component.events[2].id);
  }));

  it('signUp should be valid', async(async () => {

    //setup signup form
    component.signUpForm.controls['firstName'].setValue('Test A');
    component.signUpForm.controls['lastName'].setValue('Test B');
    component.signUpForm.controls['emailAddress'].setValue('testA@testB.com');
    component.signUpForm.controls['event'].setValue(<Event>{ id: 3 });
    component.signUpForm.controls['comments'].setValue('Test A Comments for the event');

    await component.onSubmit();

    expect(component.signUpResult.result).toBeTruthy();
    expect(component.signUpResult.errorMessage).toEqual('');
    expect(component.signUpResult.employeeSignUp.firstName).toEqual('Test A');
    expect(component.signUpResult.employeeSignUp.lastName).toEqual('Test B');
    expect(component.signUpResult.employeeSignUp.emailAddress).toEqual('testA@testB.com');
    expect(component.signUpResult.employeeSignUp.eventId).toEqual(3);
    expect(component.signUpResult.employeeSignUp.comments).toEqual('Test A Comments for the event');

  }));

  it('should call the onSubmit method', async(() => {

    fixture.detectChanges();
    spyOn(component, 'onSubmit');

    let el = fixture.debugElement.query(By.css('button')).nativeElement;
    el.click();

    expect(component.onSubmit).toHaveBeenCalledTimes(0);
  }));

});
