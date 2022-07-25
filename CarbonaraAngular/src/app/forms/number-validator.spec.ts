import { createControl } from "./forms-general";
import { NUMBER_INVALID_ERROR, ValidateNumber } from "./number-validator";

describe('NumberValidator', () => {

    let numValidator = ValidateNumber();

    it('Valid numbers', () => {
        let validControl1 = createControl("1");
        let validControl3 = createControl("123");
        let validControl5 = createControl("12345");

        expect(numValidator(validControl1)).toBeNull();
        expect(numValidator(validControl3)).toBeNull();
        expect(numValidator(validControl5)).toBeNull();
    });

    it('Numbers with space', () => {
        let numberWithHeadingSpace = createControl(" 123");
        expect(numValidator(numberWithHeadingSpace)).toBe(NUMBER_INVALID_ERROR);

        let numberWithTrailingSpace = createControl("123 ");
        expect(numValidator(numberWithTrailingSpace)).toBe(NUMBER_INVALID_ERROR);
        let numberWithInnerSpace = createControl("1 23");
        expect(numValidator(numberWithInnerSpace)).toBe(NUMBER_INVALID_ERROR);
    });

    it('Numbers with other characters', () => {
        let specialChars = createControl("1!");
        expect(numValidator(specialChars)).toBe(NUMBER_INVALID_ERROR);

        let numberA = createControl("1A");
        expect(numValidator(numberA)).toBe(NUMBER_INVALID_ERROR);
    });
});