import { Component } from '@angular/core';
import { Router } from '@angular/router';

@Component({
  selector: 'app-nav-menu',
  templateUrl: './nav-menu.component.html',
  styleUrls: ['./nav-menu.component.css']
})
export class NavMenuComponent {
  isExpanded = false;

  constructor(private router: Router) { }

  /** This function will route to the Sign Up page */
  signUp() {
    this.router.navigate(['Sign-Up']);
  }

  /** This function collapses the navbar */
  collapse() {
    this.isExpanded = false;
  }

  /** This function toggles the navbar */
  toggle() {
    this.isExpanded = !this.isExpanded;
  }
}
