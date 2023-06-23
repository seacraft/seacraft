import { NzModalRef } from 'ng-zorro-antd/modal';

import { Component, Input, OnInit } from '@angular/core';
import { FormBuilder, FormGroup, Validators } from '@angular/forms';

import { Option } from '../../../../../../shared/commonType';
import { Data } from '../../../table1/table1.component';

@Component({
  selector: 'app-edit-product',
  templateUrl: './edit-product.component.html',
  styleUrls: ['./edit-product.component.less'],
})
export class EditProductComponent implements OnInit {
  @Input() data!: Data;
  validateForm!: FormGroup;
  typeOptions: Option[] = [
    { name: 'All', value: 0 },
    { name: 'Product', value: 1 },
    { name: 'Component', value: 2 },
    { name: 'middleware', value: 3 },
  ];
  constructor(private fb: FormBuilder, private modal: NzModalRef) {}

  ngOnInit(): void {
    const group = {
      name: [this.data?.name, Validators.required],
      type: [this.data?.type.value],
      age: [this.data?.age, Validators.required],
      address: [this.data?.address],
    };

    this.validateForm = this.fb.group(group);
  }
  onCancel() {
    this.modal.triggerCancel();
  }
  submitForm() {
    Object.assign(this.data, this.validateForm.value);
    const type = this.typeOptions.find(
      (q) => q.value == this.validateForm.get('type')?.value
    );
    if (type != undefined) {
      this.data.type = type;
    }
    this.modal.triggerOk();
  }
}
