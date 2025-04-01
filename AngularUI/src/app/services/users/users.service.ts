import { HttpClient } from '@angular/common/http';
import { Injectable } from '@angular/core';
import { Observable } from 'rxjs';
import { UsersResponse } from './users-response.model';

@Injectable({
  providedIn: 'root'
})
export class UsersService {

  constructor(private http: HttpClient) { }

  getVehicleByUser(username : string): Observable<UsersResponse> {
    const body = { name: username };
    return this.http.post<UsersResponse>('https://localhost:7146/Users', body);
  }
}
