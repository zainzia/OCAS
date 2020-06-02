import { BrowserModule } from '@angular/platform-browser';
import { NgModule } from '@angular/core';
import { FormsModule } from '@angular/forms';
import { HttpClientModule, HTTP_INTERCEPTORS } from '@angular/common/http';
import { RouterModule } from '@angular/router';

import { BrowserAnimationsModule } from '@angular/platform-browser/animations';

import { AppComponent } from './app.component';
import { NavMenuComponent } from './components/nav-menu/nav-menu.component';
import { HomeComponent } from './components/home/home.component';

import { ReactiveFormsModule } from '@angular/forms';
import { SignUpComponent } from './components/sign-up/sign-up.component';
import { EventDetailComponent } from './components/event-detail/event-detail.component';

import { MatTableModule } from '@angular/material/table';
import { MatSelectModule } from '@angular/material/select';
import { MatInputModule } from '@angular/material/input';
import { MatIconModule } from '@angular/material/icon';
import { MatDialogModule, MAT_DIALOG_DEFAULT_OPTIONS } from '@angular/material/dialog';
import { MatFormFieldModule } from '@angular/material/form-field';



import { EventService } from './services/event.service';
import { EventsTableComponent } from './components/events-table/events-table.component';
import { SignUpService } from './services/sign-up.service';
import { SignUpResultDialogComponent } from './components/sign-up-result-dialog/sign-up-result-dialog.component';
import { EventDetailsResultDialogComponent } from './components/event-details-result-dialog/event-details-result-dialog.component';


@NgModule({
  declarations: [
    AppComponent,
    NavMenuComponent,
    HomeComponent,
    SignUpComponent,
    EventDetailComponent,
    EventsTableComponent,
    SignUpResultDialogComponent,
    EventDetailsResultDialogComponent
  ],
  imports: [
    BrowserModule.withServerTransition({ appId: 'ng-cli-universal' }),
    HttpClientModule,
    BrowserAnimationsModule,
    ReactiveFormsModule,
    FormsModule,
    MatTableModule,
    MatFormFieldModule,
    MatSelectModule,
    MatInputModule,
    MatIconModule,
    MatDialogModule,
    RouterModule.forRoot([
      { path: '', component: HomeComponent, pathMatch: 'full' },
      { path: 'Sign-Up', component: SignUpComponent, pathMatch: 'full' },
      { path: 'Event-Details', component: EventDetailComponent, pathMatch: 'full' },
      { path: 'Event-Details/:id', component: EventDetailComponent, pathMatch: 'full' }
    ])
  ],
  entryComponents: [
    SignUpResultDialogComponent,
    EventDetailsResultDialogComponent
  ],
  providers: [
    EventService,
    SignUpService,
    { provide: 'BASE_URL', useFactory: getBaseUrl },
    { provide: MAT_DIALOG_DEFAULT_OPTIONS, useValue: { hasBackdrop: false } }
  ],
  bootstrap: [AppComponent]
})
export class AppModule { }

export function getBaseUrl() {
  return document.getElementsByTagName('base')[0].href;
}
