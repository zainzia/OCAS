
import { TestBed, async, ComponentFixture, ComponentFixtureAutoDetect } from '@angular/core/testing';
import { BrowserModule, By } from "@angular/platform-browser";
import { EventDetailComponent } from './event-detail.component';

import { ActivatedRoute } from '@angular/router';
import { MAT_DIALOG_DEFAULT_OPTIONS, MatDialogModule } from '@angular/material/dialog';
import { of } from 'rxjs';

import { EventMockService } from '../../mock-services/eventMock.service';
import { EventService } from '../../services/event.service';
import { FormsModule, ReactiveFormsModule} from '@angular/forms';
import { MatTableModule } from '@angular/material/table';
import { MatSelectModule } from '@angular/material/select';
import { MatInputModule } from '@angular/material/input';
import { MatIconModule } from '@angular/material/icon';
import { BrowserAnimationsModule } from '@angular/platform-browser/animations';
import { Event } from '../../models/Event';

let component: EventDetailComponent;
let fixture: ComponentFixture<EventDetailComponent>;

describe('eventDetail component', () => {
  beforeEach(async(() => {
      TestBed.configureTestingModule({
        declarations: [
          EventDetailComponent
        ],
        imports: [
          BrowserModule,
          BrowserAnimationsModule,
          MatDialogModule,
          FormsModule,
          ReactiveFormsModule,
          MatTableModule,
          MatSelectModule,
          MatInputModule,
          MatIconModule,
          MatDialogModule,
        ],
        providers: [
          { provide: ComponentFixtureAutoDetect, useValue: true },
          { provide: EventService, useClass: EventMockService },
          { provide: ActivatedRoute, useValue: { params: of({ id: 3 }) } },
          { provide: MAT_DIALOG_DEFAULT_OPTIONS, useValue: { hasBackdrop: false } }
        ]
      });

      fixture = TestBed.createComponent(EventDetailComponent);
      component = fixture.componentInstance;
  }));

  it('event should be valid', async(() => {
    component.eventChangeSubscriptionHandler();
    expect(3).toEqual(component.selectedEvent.id);
  }));

  it('event should be valid', async(() => {

    component.getEvents();

    expect(3).toEqual(component.events[0].id);
    expect(4).toEqual(component.events[1].id);
    expect(5).toEqual(component.events[2].id);
  }));

  it('form should be valid', async(() => {

    component.createForm();

    expect(component.eventDetailsForm.get('event').valid).toBeFalsy();
    expect(component.eventDetailsForm.get('emailAddress').valid).toBeFalsy();
  }));

  it('should call the onSubmit method', async(() => {

    fixture.detectChanges();
    spyOn(component, 'onSubmit');

    let el = fixture.debugElement.query(By.css('button')).nativeElement;
    el.click();

    expect(component.onSubmit).toHaveBeenCalledTimes(0);
  }));

  it('employees should be valid', async(async () => {

    //setup signup form
    component.eventDetailsForm.controls['event'].setValue(<Event>{ id: 3 });
    component.eventDetailsForm.controls['emailAddress'].setValue('testA@testB.com');

    await component.onSubmit();

    expect(component.eventDetailsResponse.result).toBeTruthy();
    expect(component.eventDetailsResponse.errorMessage).toEqual('');
    expect(component.eventDetailsResponse.employees[0].id).toEqual(3);
    expect(component.eventDetailsResponse.employees[1].id).toEqual(4);
    expect(component.eventDetailsResponse.employees[2].id).toEqual(5);

  }));

});
