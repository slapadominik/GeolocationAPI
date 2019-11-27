﻿using System;
using AutoMapper;
using GeolocationAPI.Converters.Interfaces;
using GeolocationAPI.DTO.Remote;
using GeolocationAPI.Persistence.Entities;

namespace GeolocationAPI.Converters
{
    public class GeolocationDataConverter : IGeolocationDataConverter
    {
        private readonly IMapper _mapper;

        public GeolocationDataConverter()
        {
            var mapperConfig = new MapperConfiguration(cfg => { cfg.CreateMap<RemoteGeolocationData, GeolocationData>(); });
            _mapper = mapperConfig.CreateMapper();
        }

        public GeolocationData Convert(RemoteGeolocationData remoteGeolocationData)
        {
            return _mapper.Map<GeolocationData>(remoteGeolocationData);
        }
    }
}