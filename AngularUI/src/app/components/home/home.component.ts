import { Component } from '@angular/core';
import { UsersService } from '../../services/users/users.service';
import { FormsModule } from '@angular/forms';
import { NgSwitch, NgSwitchCase, NgSwitchDefault } from '@angular/common';
import { NgIf } from '@angular/common';

@Component({
  selector: 'app-home',
  standalone: true,
  imports: [FormsModule, NgSwitchCase, NgSwitchDefault, NgIf, NgSwitch],
  template: `
  <div style="margin-bottom: 15px;">
    <input type="text" placeholder="username" [(ngModel)]="username" style="padding: 8px; border: 1px solid #ccc; border-radius: 4px; width: 250px;">
  </div>
  <div>
    <button (click)="fetchVehicle()" style="padding: 10px 15px; background-color: #007bff; color: white; border: none; border-radius: 4px; cursor: pointer; margin-right: 10px;">
      Send request
    </button>
    <div *ngIf="response" [ngSwitch]="response.includes('Error')">
    <p *ngSwitchCase="true" style="color: red; margin-top: 10px;">
      {{ response }}
    </p>
    <p *ngSwitchDefault style="color: #28a745; margin-top: 10px;">
      {{ response }}
    </p>
    </div>
  </div>
  `
})

export class HomeComponent {
  username: string = '';
  response: string = '';

  constructor(private usersService: UsersService) { }

  fetchVehicle() {
    if (!this.username.trim()) {
      this.response = 'Username is required for the api call';
      return;
    }
    this.usersService.getVehicleByUser(this.username).subscribe({
      next: (res) => {
        this.response = res.message ?? 'Got a response without a message';
      },
      error: (error) => {
        console.error('uh oh, error', error);
        this.response = `Error: ${error.message}`;
      },
      complete: () => {
        console.info(`fetchVehicle is done.`)
      }
    });
  }
}