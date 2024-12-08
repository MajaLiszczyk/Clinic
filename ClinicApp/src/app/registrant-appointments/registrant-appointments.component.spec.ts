import { ComponentFixture, TestBed } from '@angular/core/testing';

import { RegistrantAppointmentsComponent } from './registrant-appointments.component';

describe('RegistrantAppointmentsComponent', () => {
  let component: RegistrantAppointmentsComponent;
  let fixture: ComponentFixture<RegistrantAppointmentsComponent>;

  beforeEach(async () => {
    await TestBed.configureTestingModule({
      imports: [RegistrantAppointmentsComponent]
    })
    .compileComponents();
    
    fixture = TestBed.createComponent(RegistrantAppointmentsComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
