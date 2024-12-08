import { ComponentFixture, TestBed } from '@angular/core/testing';

import { GetSpecialisationsComponent } from './get-specialisations.component';

describe('GetSpecialisationsComponent', () => {
  let component: GetSpecialisationsComponent;
  let fixture: ComponentFixture<GetSpecialisationsComponent>;

  beforeEach(async () => {
    await TestBed.configureTestingModule({
      imports: [GetSpecialisationsComponent]
    })
    .compileComponents();
    
    fixture = TestBed.createComponent(GetSpecialisationsComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
