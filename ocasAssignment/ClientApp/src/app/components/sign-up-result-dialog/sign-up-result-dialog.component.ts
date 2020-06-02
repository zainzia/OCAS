import { Component, Inject } from '@angular/core';
import { MatDialogRef, MAT_DIALOG_DATA } from '@angular/material/dialog';
import { EmployeeSignUpResult } from '../../models/employeeSignUpResult';


@Component({
    selector: 'app-sign-up-result-dialog',
    templateUrl: './sign-up-result-dialog.component.html',
    styleUrls: ['./sign-up-result-dialog.component.css']
})
/** sign-up-result-dialog component*/
export class SignUpResultDialogComponent {

  constructor(public dialogRef: MatDialogRef<SignUpResultDialogComponent>,
    @Inject(MAT_DIALOG_DATA) public data: EmployeeSignUpResult) { }

}
