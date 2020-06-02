
import { Injectable } from '@angular/core';

import { EmployeeSignUp } from '../models/EmployeeSignUp';
import { EmployeeSignUpResult } from '../models/employeeSignUpResult';

@Injectable()
export class SignUpMockService {

  addEmployeeToEvent(employeeSignUp: EmployeeSignUp) : Promise<EmployeeSignUpResult> {

    return new Promise((resolve, reject) => {
      let employeeSignUpResult = <EmployeeSignUpResult>{
        result: true,
        errorMessage: '',
        employeeSignUp: <EmployeeSignUp>{
          eventId: 3,
          firstName: 'Test A',
          lastName: 'Test B',
          emailAddress: 'testA@testB.com',
          comments: 'Test A Comments for the event'
        }
      };

      resolve(employeeSignUpResult);
    });
  }
}

