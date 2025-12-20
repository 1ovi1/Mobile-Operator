using System;
using System.Collections.Generic;
using System.Linq;
using System.ComponentModel;
using Microsoft.EntityFrameworkCore;
using MobileOperator.Domain.Entities;
using MobileOperator.models;
using MobileOperator.Models;

namespace MobileOperator.models
{
    class DetailingModel : INotifyPropertyChanged
    {
        private List<CallModel> allCalls;
        private readonly Infrastructure.MobileOperator _context;

        public DetailingModel(int clientId, string clientNumber, DateTime from, DateTime till, Infrastructure.MobileOperator context)
        {
            _context = context;
            allCalls = new List<CallModel>();
            SearchCalls(clientId, clientNumber, from, till);
        }
        
        public DetailingModel(string clientName, string clientNumber, DateTime from, DateTime till, Infrastructure.MobileOperator context)
        {
            _context = context;
            allCalls = new List<CallModel>();
            ClientModel client = new ClientModel(_context);

            if (!string.IsNullOrEmpty(clientName) && !string.IsNullOrEmpty(clientNumber))
            {
                var c = _context.Client.FirstOrDefault(i => i.Number == clientNumber);
                if (c != null)
                {
                    var fl = _context.FL.FirstOrDefault(f => f.UserId == c.UserId);
                    var ul = _context.UL.FirstOrDefault(u => u.UserId == c.UserId);

                    if (fl != null && fl.FIO == clientName)
                        client = new FLModel(fl, _context);
                    else if (ul != null && ul.OrganizationName == clientName)
                        client = new ULModel(ul, _context);
                }
            }
            else
            {
                if (!string.IsNullOrEmpty(clientName))
                {
                    var fl = _context.FL.FirstOrDefault(i => i.FIO == clientName);
                    if (fl != null)
                        client = new FLModel(fl, _context);
                    else
                    {
                        var ul = _context.UL.FirstOrDefault(i => i.OrganizationName == clientName);
                        if (ul != null)
                            client = new ULModel(ul, _context);
                    }
                }
                if (!string.IsNullOrEmpty(clientNumber))
                {
                    var c = _context.Client.FirstOrDefault(i => i.Number == clientNumber);
                    if (c != null)
                    {
                        var fl = _context.FL.FirstOrDefault(f => f.UserId == c.UserId);
                        if (fl != null)
                            client = new FLModel(fl, _context);
                        else
                        {
                            var ul = _context.UL.FirstOrDefault(u => u.UserId == c.UserId);
                            if (ul != null)
                                client = new ULModel(ul, _context);
                        }     
                    }
                }
            }

            if (client.Id != 0)
            {
                SearchCalls(client.Id, client.Number, from, till);
            }
        }

        private void SearchCalls(int clientId, string clientNumber, DateTime from, DateTime till)
        {
            var rawCalls = _context.Call
                .Include(c => c.CallType)
                .Where(i => (i.CallerId == clientId || i.CalledId == clientId || i.CalledNumber == clientNumber || i.CallerNumber == clientNumber)
                            && i.CallTime <= till && i.CallTime >= from)
                .OrderByDescending(i => i.CallTime)
                .ToList();

            allCalls = new List<CallModel>();

            foreach (var callEntity in rawCalls)
            {
                var call = new CallModel(callEntity);

                if (callEntity.CallerId == clientId || callEntity.CallerNumber == clientNumber)
                {
                    call.Direction = "Исходящий";
                    if (callEntity.CalledId != null && callEntity.CalledId != 0)
                    {
                        var calledClient = _context.Client.FirstOrDefault(x => x.UserId == callEntity.CalledId);
                        if (calledClient != null)
                        {
                            ClientModel c = new ClientModel(calledClient, _context);
                            call.Number = c.Number;
                        }
                        else
                        {
                            call.Number = callEntity.CalledNumber;
                        }
                    }
                    else
                        call.Number = callEntity.CalledNumber;
                }
                else
                {
                    call.Direction = "Входящий";
                    if (callEntity.CallerId != null && callEntity.CallerId != 0)
                    {
                        var callerClient = _context.Client.FirstOrDefault(x => x.UserId == callEntity.CallerId);
                        if (callerClient != null)
                        {
                            ClientModel c = new ClientModel(callerClient, _context);
                            call.Number = c.Number;
                        }
                        else
                        {
                            call.Number = callEntity.CallerNumber;
                        }
                    }
                    else
                        call.Number = callEntity.CallerNumber;
                }
                allCalls.Add(call);
            }
        }

        public List<CallModel> AllCalls
        {
            get { return allCalls; }
        }

        public event PropertyChangedEventHandler PropertyChanged;
        public void OnPropertyChanged(string prop = "")
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(prop));
        }
    }
}
