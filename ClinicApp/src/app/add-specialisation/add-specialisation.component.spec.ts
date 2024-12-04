import { ComponentFixture, TestBed } from '@angular/core/testing';

import { AddSpecialisationComponent } from './add-specialisation.component';

describe('AddSpecialisationComponent', () => {
  let component: AddSpecialisationComponent;
  let fixture: ComponentFixture<AddSpecialisationComponent>;

  beforeEach(async () => {
    await TestBed.configureTestingModule({
      imports: [AddSpecialisationComponent]
    })
    .compileComponents();
    
    fixture = TestBed.createComponent(AddSpecialisationComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
