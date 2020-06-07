import { Component, OnInit, Input } from '@angular/core';
import { FormBuilder, FormGroup, Validators, AbstractControl } from '@angular/forms';

@Component({
  selector: 'app-order-add',
  templateUrl: './order-add.component.html',
  styleUrls: ['./order-add.component.css']
})
export class OrderAddComponent implements OnInit {

  orderForm: FormGroup;
  
  // now = new Date();
  // realizationDate = new Date();
  service: AbstractControl;
  orderDate: AbstractControl;
  realizationDate: AbstractControl;
  price: AbstractControl;
  isPayed: AbstractControl;
  

  constructor(formBuilder: FormBuilder) {
    this.orderForm = formBuilder.group({
      'service': ['', Validators.required],
      'orderDate': [new Date(), Validators.required],
      'realizationDate': ['', Validators.required],
      'price': ['', Validators.required],
      'isPayed': ['', Validators.required]
    });

   this.service = this.orderForm.controls['service'];
   this.orderDate = this.orderForm.controls['orderDate'];
   this.realizationDate = this.orderForm.controls['realizationDate'];
   this.price = this.orderForm.controls['price'];
   this.isPayed = this.orderForm.controls['isPayed'];
  }

  onSubmit(value: String): void{
    console.log(value)
  }

  ngOnInit(): void {
    // this.realizationDate.setDate(this.realizationDate.getDate() + 3);
    // this.orderDate = new Date(); //.toISOString().substring(0, 10);
    // this.orderForm.controls['orderForm'].setValue(this.orderDate);
  }

}
