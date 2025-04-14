import { ComponentFixture, TestBed } from '@angular/core/testing';
import { UsersService } from '../../services/users/users.service';

import { HomeComponent } from './home.component';
import { CommonModule } from '@angular/common';
import { FormsModule } from '@angular/forms';

describe('HomeComponent', () => {
  let component: HomeComponent;
  let fixture: ComponentFixture<HomeComponent>;
  let usersServiceMock: jasmine.SpyObj<UsersService>;
  
  beforeEach(async () => {
    usersServiceMock = jasmine.createSpyObj('UsersService', ['getVehicleByUser']);

    await TestBed.configureTestingModule({
      imports: [HomeComponent, FormsModule, CommonModule],
      providers: [{ provide: UsersService, useValue: usersServiceMock } ]
    }).compileComponents();

    fixture = TestBed.createComponent(HomeComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
