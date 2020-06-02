import { Employee } from "./Employee";

export interface EventDetailsResponseDTO {

  result: boolean;

  errorMessage: string;

  employees: Employee[];

}
