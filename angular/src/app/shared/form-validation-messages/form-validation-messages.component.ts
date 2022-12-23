import { Component, Input, OnInit } from '@angular/core';

@Component({
  selector: 'form-validation-messages',
  templateUrl: './form-validation-messages.component.html',
  styleUrls: ['./form-validation-messages.component.scss'],
})
export class FormValidationMessagesComponent implements OnInit {
  
  @Input()
  messageType: string

  @Input()
  message: string
  
  ngOnInit(): void {
    setTimeout(() => {
      this.message = null;
    }, 4000);
  }
}
