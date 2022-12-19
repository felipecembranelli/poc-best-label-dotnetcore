const axios = require("axios");

const BASE_URL = `http://localhost:9000/api`

module.exports = {
    getCompatibility: (yourName, yourBirthday, theirName, theirBirthday) => axios({
        method:"POST",
        url : BASE_URL + `/zodiac_compatibility/result/`,
        headers: {
            "content-type":"application/x-www-form-urlencoded",
            "x-rapidapi-host":"astrology-horoscope.p.rapidapi.com",
            "x-rapidapi-key": "yourapikey"
        },
        params: {
            mystic_dob: yourBirthday,
            mystic_dob2: theirBirthday,
            mystic_name: yourName,
            mystic_name2: theirName
        }
    }),


    httpGetCars: () => axios({
        method:"GET",
        url : BASE_URL + `/cars/`
    })
}