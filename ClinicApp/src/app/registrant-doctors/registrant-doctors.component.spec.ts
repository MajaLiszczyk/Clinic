import { ComponentFixture, TestBed } from '@angular/core/testing';

import { RegistrantDoctorsComponent } from './registrant-doctors.component';

describe('RegistrantDoctorsComponent', () => {
  let component: RegistrantDoctorsComponent;
  let fixture: ComponentFixture<RegistrantDoctorsComponent>;

  beforeEach(async () => {
    await TestBed.configureTestingModule({
      imports: [RegistrantDoctorsComponent]
    })
    .compileComponents();
    
    fixture = TestBed.createComponent(RegistrantDoctorsComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
