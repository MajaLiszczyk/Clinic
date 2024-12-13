import { ComponentFixture, TestBed } from '@angular/core/testing';

import { RegistrantPatientsOldComponent } from './registrant-patients-old.component';

describe('RegistrantPatientsOldComponent', () => {
  let component: RegistrantPatientsOldComponent;
  let fixture: ComponentFixture<RegistrantPatientsOldComponent>;

  beforeEach(async () => {
    await TestBed.configureTestingModule({
      imports: [RegistrantPatientsOldComponent]
    })
    .compileComponents();
    
    fixture = TestBed.createComponent(RegistrantPatientsOldComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
