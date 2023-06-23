import { NzModalRef } from 'ng-zorro-antd/modal';

import { Component, Input, OnInit } from '@angular/core';
import { FormBuilder, FormGroup, Validators } from '@angular/forms';

import { Option } from '../../../../../../shared/commonType';
import { Data } from '../../../table1/table1.component';

@Component({
  selector: 'app-create-product',
  templateUrl: './create-product.component.html',
  styleUrls: ['./create-product.component.less']
})
export class CreateProductComponent implements OnInit {

  validateForm!: FormGroup;
  typeOptions: Option[] = [
    { name: 'Product', value: 1 },
    { name: 'Component', value: 2 },
    { name: 'middleware', value: 3 },
  ];
  
  constructor(
    private fb: FormBuilder,
     private modal: NzModalRef) {}

  ngOnInit(): void {
    const group = {
      name: ['',Validators.required],
      type: [1],
      age: [1, Validators.required],
      address: [''],
    };
    this.validateForm = this.fb.group(group);
  }
  get data():Data{
      const d = this.validateForm.value as Data;
      const type = this.typeOptions.find(
        (q) => q.value == this.validateForm.get('type')?.value
      );
      if (type != undefined) {
        d.type = type;
      }
      return d;
  }
  onCancel() {
    this.modal.triggerCancel();
  }
  submitForm() {
    this.modal.triggerOk();
  }
}
