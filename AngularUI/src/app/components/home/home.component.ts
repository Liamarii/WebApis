import { Component, OnInit, signal } from '@angular/core';
import { UsersService } from '../../services/users/users.service';
import { FormsModule } from '@angular/forms';
import { CommonModule } from '@angular/common';

@Component({
  selector: 'app-home',
  standalone: true,
  imports: [FormsModule, CommonModule],
  templateUrl: './home.component.html',
  styleUrl: './home.component.scss'
})

export class HomeComponent implements OnInit{
  username = signal<string>('');
  response = signal<string>('');
  error = signal<string>('');
  timeOfPageLoad = signal<string>('');

  constructor(private readonly usersService: UsersService) { }
  
  ngOnInit(): void {
    this.timeOfPageLoad.set(new Date().toLocaleTimeString('en-GB', {
      timeZone: 'Europe/London',
      hour12: false
    }));
  }

  fetchVehicle(): void {
    if (!this.username().trim()) {
      this.error.set('Username is required for the api call');
      return;
    }
    this.usersService.getVehicleByUser(this.username()).subscribe({
      next: (res) => {
        this.response.set(res.message ?? 'Got a response without a message');
      },
      error: (error) => {
        this.error.set(error.message);
        console.error(error);
      },
      complete: () => {
        console.info(`getVehicleByUser call is done.`);
      }
    });
  }
}