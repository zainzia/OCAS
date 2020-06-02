
import { Injectable } from '@angular/core';

import { Event } from '../models/Event';
import { EventDetailsDTO } from '../models/EventDetailsDTO';
import { EventDetailsResponseDTO } from '../models/EventDetailsResponseDTO';
import { Employee } from '../models/Employee';

@Injectable()
export class EventMockService {

  getEvent(id: number): Promise<Event> {
    return new Promise((resolve, reject) => {
      let event = <Event>{
        id: 3
      };
      resolve(event);
    });
  }

  getAllEvents(): Promise<Event[]> {
    return new Promise((resolve, reject) => {
      let events = <Event[]>[];

      events.push(<Event>{ id: 3 });
      events.push(<Event>{ id: 4 });
      events.push(<Event>{ id: 5 });

      resolve(events);
    });
  }

  getEmployeesForEvent(eventDetails: EventDetailsDTO): Promise<EventDetailsResponseDTO> {
    return new Promise((resolve, reject)=> {

      let eventDetailsResponseDTO = <EventDetailsResponseDTO>{
        result: true,
        errorMessage: '',
        employees: <Employee[]>[<Employee>{ id: 3 }, <Employee>{ id: 4 }, <Employee>{ id: 5 }]
      };

      resolve(eventDetailsResponseDTO);
    });
  }

}
