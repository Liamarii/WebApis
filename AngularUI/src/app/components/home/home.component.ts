import { Component, OnInit } from '@angular/core';
import { UsersResponse } from '../../services/users/users-response.model';
import { UsersService } from '../../services/users/users.service';

@Component({
  selector: 'app-home',
  standalone: true,
  imports: [],
  templateUrl: './home.component.html',
  styleUrl: './home.component.scss'
})
export class HomeComponent implements OnInit {
  userVehicle?: UsersResponse;

  constructor(private usersService: UsersService) {}

  ngOnInit(): void {
    this.usersService.getVehicleByUser().subscribe({
      next: (response: UsersResponse) => {
        console.log('response', response);
        this.userVehicle = response;
      },
      error: (error) => {
        console.error('Error:', error);
      }
    });
  }
}
