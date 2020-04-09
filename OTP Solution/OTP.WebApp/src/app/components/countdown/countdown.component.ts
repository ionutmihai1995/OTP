import { Component, OnInit, Input, SimpleChanges } from '@angular/core';

@Component({
  selector: 'app-countdown',
  templateUrl: './countdown.component.html',
  styleUrls: [
    './countdown.component.scss'
  ],
})
export class CountdownComponent implements OnInit {

  @Input() timerSeconds: number;
  timer: any;
  constructor() { }

  ngOnChanges(changes: SimpleChanges){
    if (changes['timerSeconds']) {
      if (changes['timerSeconds'].currentValue) {
        this.startTimer();
      }
    }
  }

  ngOnInit(): void {

  }

  startTimer() {
    this.timer = setInterval(function () {
      this.timerSeconds -= 0.1;
      if (this.timerSeconds <= 0) {
        this.timerSeconds = null;
      }
    }.bind(this), 100);
  }
}
