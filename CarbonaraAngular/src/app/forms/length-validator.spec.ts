import { createControl } from './forms-general';
import { LENGTH_INVALID_ERROR, ValidateLength } from './length-validator';

/*
        ValidatorFn returns an object, if there was an 
        error while validating. Null is returned, 
        if the validator is fine with the input.
    */

describe('LengthValidator', () => {

  beforeEach(async () => {
    
  });

  it('Check valid length', () => {
    let validator = ValidateLength(5);

    //Check valid input
    let controlValid = createControl("12345"); //No error
    expect(validator(controlValid)).toBeNull();
  });

  it('Check invalid length', () => {
    let validator = ValidateLength(5);
    //Check length 4
    let control4 = createControl("1234");
    expect(validator(control4)).toBe(LENGTH_INVALID_ERROR); //Error: length should be 5, but is 4
    //Check length 5
    let control6 = createControl("123456");
    expect(validator(control6)).toBe(LENGTH_INVALID_ERROR); //Error: length should be 5, but is 6
  });

  it('Check length=0', () => {
    //Check unusual validators
    let validator0 = ValidateLength(0);
    let control1 = createControl("1");
    expect(validator0(control1)).toBe(LENGTH_INVALID_ERROR); //Error: length should be 0, but is 1
    let control0 = createControl("");
    expect(validator0(control0)).toBeNull(); //No error
  });

  it('Check negative length', () => {
    let validatorNeg1 = ValidateLength(-1);

    let control0 = createControl("");
    let control1 = createControl("1");

    expect(validatorNeg1(control1)).toBe(LENGTH_INVALID_ERROR); //There should be an error with length=1 and length=0, because expected is -1
    expect(validatorNeg1(control0)).toBe(LENGTH_INVALID_ERROR);
  });


});




