
import { TestBed, async, ComponentFixture, ComponentFixtureAutoDetect } from '@angular/core/testing';
import { BrowserModule } from "@angular/platform-browser";

import { RouterModule } from '@angular/router';

import { EventMockService } from '../../mock-services/eventMock.service';
import { EventService } from '../../services/event.service';
import { FormsModule, ReactiveFormsModule } from '@angular/forms';
import { MatTableModule } from '@angular/material/table';
import { MatSelectModule } from '@angular/material/select';
import { MatFormFieldModule } from '@angular/material/form-field';
import { MatOptionModule } from '@angular/material/core';
import { MatIconModule } from '@angular/material/icon';
import { BrowserAnimationsModule } from '@angular/platform-browser/animations';

import { EventsTableComponent } from './events-table.component';
import { EventDetailComponent } from '../event-detail/event-detail.component';

let component: EventsTableComponent;
let fixture: ComponentFixture<EventsTableComponent>;

describe('eventsTable component', () => {
  beforeEach(async(() => {
    TestBed.configureTestingModule({
      declarations: [
        EventsTableComponent,
        EventDetailComponent
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
        RouterModule.forRoot([
          { path: 'Event-Details/:id', component: EventDetailComponent, pathMatch: 'full' }
        ])
      ],
      providers: [
        { provide: ComponentFixtureAutoDetect, useValue: true },
        { provide: EventService, useClass: EventMockService }
      ]
    });

    fixture = TestBed.createComponent(EventsTableComponent);
    component = fixture.componentInstance;
  }));

  it('events should be valid', async(() => {
    component.getAllEvents();
    expect(3).toEqual(component.events[0].id);
    expect(4).toEqual(component.events[1].id);
    expect(5).toEqual(component.events[2].id);
  }));

});
