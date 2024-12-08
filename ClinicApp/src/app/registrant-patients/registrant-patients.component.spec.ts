import { ComponentFixture, TestBed } from '@angular/core/testing';

import { RegistrantPatientsComponent } from './registrant-patients.component';

describe('RegistrantPatientsComponent', () => {
  let component: RegistrantPatientsComponent;
  let fixture: ComponentFixture<RegistrantPatientsComponent>;

  beforeEach(async () => {
    await TestBed.configureTestingModule({
      imports: [RegistrantPatientsComponent]
    })
    .compileComponents();
    
    fixture = TestBed.createComponent(RegistrantPatientsComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
