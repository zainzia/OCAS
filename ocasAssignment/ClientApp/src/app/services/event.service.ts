
import { Injectable } from '@angular/core';
import { NodeService } from './node.service';
import { Event } from '../models/Event';
import { Employee } from '../models/Employee';
import { EventDetailsDTO } from '../models/EventDetailsDTO';
import { EventDetailsResponseDTO } from '../models/EventDetailsResponseDTO';

@Injectable()
export class EventService {


  constructor(private nodeService: NodeService) { }

  /**
   * This method obtains an Event from the server defined by the Id
   * @param id The Id of the Event to obtain
   */
  getEvent(id: number): Promise<Event> {
    return this.nodeService.fetchURL(`API/Events/${id}`).then((event: Event) => {
      return event;
    });
  }

  /** This method will obtain all Events from the server */
  getAllEvents() : Promise<Event[]> {
    return this.nodeService.fetchURL('API/Events/All').then((events: Event[]) => {
      return events;
    });
  }

  /**
   * This method will obtain all Employees for an Event if the employee requesting
   * is signed up.
   * @param eventDetails The event and employee details
   */
  getEmployeesForEvent(eventDetails: EventDetailsDTO): Promise<EventDetailsResponseDTO> {
    return this.nodeService.postObject('API/Events/Employees', eventDetails).then((employees: EventDetailsResponseDTO) => {
      return employees;
    });
  }

}
