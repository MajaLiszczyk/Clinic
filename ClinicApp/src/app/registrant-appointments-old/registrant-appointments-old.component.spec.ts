import { ComponentFixture, TestBed } from '@angular/core/testing';

import { RegistrantAppointmentsOldComponent } from './registrant-appointments-old.component';

describe('RegistrantAppointmentsComponent', () => {
  let component: RegistrantAppointmentsOldComponent;
  let fixture: ComponentFixture<RegistrantAppointmentsOldComponent>;

  beforeEach(async () => {
    await TestBed.configureTestingModule({
      imports: [RegistrantAppointmentsOldComponent]
    })
    .compileComponents();
    
    fixture = TestBed.createComponent(RegistrantAppointmentsOldComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
