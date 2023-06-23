import { ComponentFixture, TestBed } from '@angular/core/testing';

import { TwoerComponent } from './twoer.component';

describe('TwoerComponent', () => {
  let component: TwoerComponent;
  let fixture: ComponentFixture<TwoerComponent>;

  beforeEach(async () => {
    await TestBed.configureTestingModule({
      declarations: [ TwoerComponent ]
    })
    .compileComponents();

    fixture = TestBed.createComponent(TwoerComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
