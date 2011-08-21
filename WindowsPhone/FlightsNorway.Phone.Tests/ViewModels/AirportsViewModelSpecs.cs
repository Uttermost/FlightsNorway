﻿using System.Linq;
using FlightsNorway.Lib.DataServices;
using FlightsNorway.Lib.Messages;
using FlightsNorway.Lib.Model;
using FlightsNorway.Lib.ViewModels;
using FlightsNorway.Tests.Stubs;
using Microsoft.Silverlight.Testing;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using TinyMessenger;

namespace FlightsNorway.Tests.ViewModels
{
    [TestClass]
    public class AirportsViewModelSpecs : SilverlightTest
    {
        [TestMethod, Tag(Tags.ViewModel)]
        public void Should_load_airports_when_viewmodel_is_created()
        {
            Assert.IsTrue(_viewModel.Airports.Count > 0);
        }

        [TestMethod, Tag(Tags.ViewModel)]
        public void First_airport_in_list_should_be_option_to_select_nearest()
        {
            Assert.AreEqual("Nærmeste flyplass", _viewModel.Airports[0].Name);
        }

        [TestMethod, Tag(Tags.ViewModel)]
        public void Saves_selected_airport()
        {
            _objectStore.Delete(Airport.SelectedAirportFilename);
            _viewModel.SelectedAirport = _viewModel.Airports.Last();
            _viewModel.SaveCommand.Execute(null);

            var airport = _objectStore.Load<Airport>(Airport.SelectedAirportFilename);

            Assert.AreEqual(_viewModel.SelectedAirport, airport);
        }

        [TestMethod, Tag(Tags.ViewModel)]
        public void Fires_change_notification_when_selecting_airport()
        {
            var airport = _viewModel.Airports.Last();

            string propertyName = string.Empty;
            _viewModel.PropertyChanged += (o, e) => propertyName = e.PropertyName;

            _viewModel.SelectedAirport = airport;

            Assert.AreEqual("SelectedAirport", propertyName);
        }
        
        [TestMethod, Tag(Tags.ViewModel)]
        public void Publishes_message_when_user_selects_an_airport()
        {
            AirportSelectedMessage lastMessage = null;
            _messenger.Subscribe<AirportSelectedMessage>(m => lastMessage = m);

            _viewModel.SelectedAirport = _viewModel.Airports.Last();
            _viewModel.SaveCommand.Execute(null);

            Assert.IsNotNull(lastMessage);
            Assert.AreEqual(_viewModel.SelectedAirport, lastMessage.Content);
        }

        [TestMethod, Tag(Tags.ViewModel)]
        public void Publishes_message_when_user_selects_nearest_airport()
        {
            FindNearestAirportMessage lastMessage = null;
            _messenger.Subscribe<FindNearestAirportMessage>(m => lastMessage = m);

            _viewModel.SelectedAirport = _viewModel.Airports.Where(a => a.Code == Airport.Nearest.Code).Single();
            _viewModel.SaveCommand.Execute(null);

            Assert.IsNotNull(lastMessage);
        }

        [TestInitialize]
        public void Setup()
        {
            _objectStore = new ObjectStoreStub();
            _airportsService = new AirportNamesService();
            _messenger = new TinyMessengerHub();
            _viewModel = new AirportsViewModel(_airportsService, _objectStore, _messenger);
        }

        private TinyMessengerHub _messenger;
        private AirportNamesService _airportsService;
        private AirportsViewModel _viewModel;
        private ObjectStoreStub _objectStore;
    }
}
