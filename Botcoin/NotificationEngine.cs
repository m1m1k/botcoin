﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Botcoin.Shared;
using Botcoin.Shared.Interfaces;
using Botcoin.Shared.Models;

namespace Botcoin
{
    public class NotificationEngine : INotificationEngine
    {
        readonly ISMTPProvider smtpProvider;

        public NotificationEngine(ISMTPProvider _smtpProvider)
        {
            smtpProvider = _smtpProvider;
        }

        public void TradeSignal(TickDataModel buyQuote, TickDataModel sellQuote)
        {
            string subject = string.Format(
            "Botcoin - Arbitrarge oppurtunity discovered between {0}, {1}",
            buyQuote.SourceExchange, sellQuote.SourceExchange);

            string body = string.Format(
@"
Botcoin has discovered an <i>arbitrarge</i> oppurtunity by buying <b>{0}</b> and selling <b>{1}</b>!<br>
<br>
Current order book for {0}:<br>
<u>Bid: <b>${2}</b></u> Ask: <b>${3}</b><br>
<br>
Current order book for {1}:<br>
Bid: <b>${4}</b> <u>Ask: <b>${5}</b></u><br>
<br>
You stand to make a profit of: ${6}<br>

"
, buyQuote.SourceExchange, sellQuote.SourceExchange,
buyQuote.Bid, buyQuote.Ask,
sellQuote.Bid, sellQuote.Ask,
sellQuote.Ask - buyQuote.Bid
);

            smtpProvider.Send(subject, body);
        }




        public void TradeSignal(Trade trade)
        {
            string subject = string.Format("Botcoin - Trade signal");

            string table = "";

            foreach (var transaction in trade.Transactions)
            {
                table += string.Format(
@"
    <tr>
        <td>
            {0}
        </td>
        <td>
        </td>
    </tr>
",
 transaction.ToString()
                    );
            }

            string body = string.Format(
@"
Botcoin trade signal: {0}</b>!<br>
<br>

<table>
    <tr>
    <th>Trade</th>
    <th></th>
    </tr>
{1}
</table>
"
, table
);

            smtpProvider.Send(subject, body);
        }
    }
}
