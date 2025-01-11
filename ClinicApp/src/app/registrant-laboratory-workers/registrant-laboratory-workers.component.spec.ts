import { ComponentFixture, TestBed } from '@angular/core/testing';

import { RegistrantLaboratoryWorkersComponent } from './registrant-laboratory-workers.component';

describe('RegistrantLaboratoryWorkersComponent', () => {
  let component: RegistrantLaboratoryWorkersComponent;
  let fixture: ComponentFixture<RegistrantLaboratoryWorkersComponent>;

  beforeEach(async () => {
    await TestBed.configureTestingModule({
      imports: [RegistrantLaboratoryWorkersComponent]
    })
    .compileComponents();
    
    fixture = TestBed.createComponent(RegistrantLaboratoryWorkersComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
