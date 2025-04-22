import { TestBed } from '@angular/core/testing';
import { UsersService } from './users.service';
import { HttpTestingController, provideHttpClientTesting } from '@angular/common/http/testing';
import { provideHttpClient, withFetch } from '@angular/common/http';

describe('UsersService', () => {
  let service: UsersService;
  let httpMock: HttpTestingController;

  beforeEach(() => {
    TestBed.configureTestingModule({
      providers: [provideHttpClientTesting(), provideHttpClient(), withFetch()] 
    });
    service = TestBed.inject(UsersService);
    httpMock = TestBed.inject(HttpTestingController); 
  });

  it('should be created', () => {
    expect(service).toBeTruthy();
  });
});
