import { HttpClient } from '@angular/common/http';
import { Injectable } from '@angular/core';
import { Observable } from 'rxjs';

@Injectable({
  providedIn: 'root'
})
export class UsersService {

  constructor(private http: HttpClient) { }

  getJoke(): Observable<Joke> {
    return this.http.get<Joke>('https://official-joke-api.appspot.com/random_joke');
  }
}