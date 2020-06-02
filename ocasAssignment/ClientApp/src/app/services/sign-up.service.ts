
import { Injectable } from '@angular/core';
import { NodeService } from './node.service';
import { EmployeeSignUp } from '../models/EmployeeSignUp';
import { EmployeeSignUpResult } from '../models/employeeSignUpResult';

@Injectable()
export class SignUpService {

  constructor(private nodeService: NodeService) { }

  /**
   * This method signs up a new employee for an event
   * @param employeeSignUp - The details of the Employee sign up
   */
  addEmployeeToEvent(employeeSignUp: EmployeeSignUp) {

    return this.nodeService.postObject('API/SignUp', employeeSignUp).then((employeeSignUpResult: EmployeeSignUpResult) => {
      return employeeSignUpResult;
    }).catch(error => {
      return <EmployeeSignUpResult>{
        result: false,
        errorMessage: error
      };
    });

  }

}
