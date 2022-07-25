import { Component, Input, OnInit } from '@angular/core';

@Component({
  selector: 'cb-icon',
  templateUrl: './carbonara-icon.component.html',
  styleUrls: ['./carbonara-icon.component.scss']
})
export class CarbonaraIconComponent implements OnInit {

  constructor() { }

  @Input() label: string = "";
  @Input() icon: string = "";

  ngOnInit(): void {
  }

}
